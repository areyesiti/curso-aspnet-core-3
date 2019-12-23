using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreGram.Data;
using CoreGram.Data.Dto;
using CoreGram.Data.Models;
using CoreGram.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreGram.Repositories
{
    public class UserRepository
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;

        public UserRepository(DataContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _context = context;            
            _mapper = mapper;

            // Obtenemos el valor las AppSettings del fichero de configuración
            _appSettings = appSettings.Value;
        }

        public async Task<AuthDto> Authenticate(LoginDto auth)
        {            
            // Comprobamos si viene informado el login y el password
            if (string.IsNullOrEmpty(auth.Login) || string.IsNullOrEmpty(auth.Password))
                throw new BadRequestException("Usuario o password incorrecto");

            // Obtenemos el usuario y el perfil del login solicitado
            var user = await _context.Users.Include(x => x.Profile).SingleOrDefaultAsync((x => x.Login == auth.Login));

            // Comprobamos si existe el usuario
            if (user == null) throw new NotFoundException("El usuario no es válido");

            // Comprobamos si coincide el password encriptándolo primero
            if (EncryptPassword(auth.Password) != user.Password) throw new BadRequestException("El password no es válido");
            
            // Convertimos la key de appsettings en un array de bytes
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            // Crea el descriptor del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // 	Establece las notificaciones de salida que se van a incluir en el token emitido.
                Subject = new ClaimsIdentity(new Claim[]
                {                    
                    //new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString())                    
                }),

                // Establece la fecha que expira el token
                Expires = DateTime.UtcNow.AddDays(7),

                // Establece las credenciales que se utilizan para firmar el token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)               
            };

            // Creamos un manejador para el token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Creamos el token en base al descriptor del token especificado
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Serializa el token
            var tokenString = tokenHandler.WriteToken(token);

            // Creamos el AuthDto de salida
            AuthDto response = new AuthDto
            {
                UserInfo = _mapper.Map<UserInfoDto>(user),
                Token = tokenString
            };            

            return response;
        }

        public async Task<IEnumerable<UserInfoDto>> GetAll()
        {
            // Obtenemos la lista de usuarios completa y mapeamos la lista al dto de salida
            var model = await _context.Users.Include(x => x.Profile).ToListAsync();
            var response = _mapper.Map<List<User>, List<UserInfoDto>>(model);
            return response;
        }

        public async Task<UserInfoDto> GetById(int userId)
        {
            // Obtenemos el usuario y comprobamos que existe
            var model = await _context.Users
                            .Where(x => x.Id == userId)
                            .Include(x => x.Profile)
                            .FirstOrDefaultAsync();

            if (model == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }

            // Mapeamos la salida
            var response = _mapper.Map<UserInfoDto>(model);
            return response;
        }

        public async Task<UserInfoDto> Create(UserDto dto)
        {
            // Validación Pasword requerido 
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new BadRequestException("El password es requerido");

            // Validación usuario existente
            if (await _context.Users.AnyAsync(x => x.Login == dto.Login))
                throw new BadRequestException("El Usuario " + dto.Login + " ya existe");

            // Encriptamos el password
            dto.Password = EncryptPassword(dto.Password);

            // Añadimos el usuario al contexto y guardamos
            var model = _mapper.Map<User>(dto);          
            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            // Devolvemos el dto de salida
            var response = _mapper.Map<UserInfoDto>(model);
            return response;
        }

        public async Task<UserInfoDto> Update(int userId, UserDto dto)
        {
            // Comprobamos que existe el usuario a actualizar
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new NotFoundException("Usuario no encontrado");           

            // Comprobamos si el nombre de usuario ha cambiado y si es así comprobamos si ya existe
            if (dto.Login != user.Login)
            {
                if (await _context.Users.AnyAsync(x => x.Login == dto.Login))
                {
                    throw new BadRequestException("Usuario " + dto.Login + " ya existe");
                }
            }            

            // Actualizamos la contraseña si viene informada
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                dto.Password = EncryptPassword(dto.Password);
            }

            //_context.Entry<User>(user).State = EntityState.Detached;

            

            // Mapeamos a la entidad, actualizamos y guardamos
            var model = _mapper.Map<User>(dto);

            _context.Entry(user).CurrentValues.SetValues(dto);

            //_context.Users.Update(model);
            
            _context.Users.Update(user);

            _context.SaveChangesAsync();
            
            // Mapeamos la salida
            var response = _mapper.Map<UserInfoDto>(model);
            return response;
        }

        public async Task<UserInfoDto> Delete(int userId)
        {
            // Obtenemos y comprobamos que existe el usuario
            var model = await _context.Users.FindAsync(userId);
            if (model != null)
            {
                // Se eliminan los registros de seguidores donde aparece el usuario
                var followers = _context.Followers.Where(x => x.FollowerId == userId);
                var followings = _context.Followers.Where(x => x.UserId == userId);
                _context.Followers.RemoveRange(followers);
                _context.Followers.RemoveRange(followings);
                _context.Users.Remove(model);
                await _context.SaveChangesAsync();

                // Mapeamos la salida
                var response = _mapper.Map<UserInfoDto>(model);
                return response;
            }
            else
            {
                throw new NotFoundException("No se ha encontrado el usuario");
            }
        }

        public User GetByName(string userName)
        {
            return _context.Users.Where(x => x.Login == userName).FirstOrDefault();
        }

        private string EncryptPassword(string password)
        {
            // Encriptamos el password recibido por parámetros
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }
    }
}
