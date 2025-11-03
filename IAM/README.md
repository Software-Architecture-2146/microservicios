# IAM Microservice

Este microservicio de Identity and Access Management (IAM) proporciona funcionalidades completas de autenticación y gestión de usuarios.

## Endpoints Disponibles

### Authentication Controller (`/api/authentication`)

1. **POST** `/sign-in` - Iniciar sesión
   - **Descripción**: Autentica un usuario con email y contraseña
   - **Request Body**:
     ```json
     {
       "email": "usuario@example.com",
       "password": "password123"
     }
     ```
   - **Response**: 
     ```json
     {
       "id": 1,
       "email": "usuario@example.com",
       "username": "usuario",
       "role": "Traveller",
       "token": "jwt_token_here"
     }
     ```

2. **POST** `/sign-up` - Registrar nuevo usuario
   - **Descripción**: Crea una nueva cuenta de usuario
   - **Request Body**:
     ```json
     {
       "email": "nuevo@example.com",
       "username": "nuevoUsuario",
       "password": "password123",
       "role": "Traveller"
     }
     ```
   - **Response**: 
     ```json
     {
       "message": "User created successfully"
     }
     ```

3. **POST** `/validate-token` - Validar token JWT
   - **Descripción**: Valida un token JWT y devuelve información del usuario
   - **Request Body**:
     ```json
     {
       "token": "jwt_token_here"
     }
     ```
   - **Response**: 
     ```json
     {
       "message": "Token is valid",
       "user": {
         "id": 1,
         "email": "usuario@example.com",
         "username": "usuario",
         "role": "Traveller"
       },
       "userId": 1
     }
     ```

### Users Controller (`/api/users`) - Requiere Autenticación

1. **GET** `/` - Obtener todos los usuarios
   - **Descripción**: Devuelve una lista de todos los usuarios
   - **Headers**: `Authorization: Bearer {jwt_token}`
   - **Response**: Array de usuarios

2. **GET** `/{id}` - Obtener usuario por ID
   - **Descripción**: Devuelve un usuario específico por su ID
   - **Headers**: `Authorization: Bearer {jwt_token}`
   - **Response**: Objeto usuario

3. **GET** `/email/{email}` - Obtener usuario por email
   - **Descripción**: Devuelve un usuario específico por su email
   - **Headers**: `Authorization: Bearer {jwt_token}`
   - **Response**: Objeto usuario

4. **GET** `/profile` - Obtener perfil del usuario autenticado
   - **Descripción**: Devuelve el perfil del usuario actualmente autenticado
   - **Headers**: `Authorization: Bearer {jwt_token}`
   - **Response**: Objeto usuario

## Configuración JWT

El microservicio está configurado con JWT para la autenticación. Las configuraciones se encuentran en:

- `appsettings.json`
- `appsettings.Development.json`

```json
{
  "TokenSettings": {
    "Secret": "SuperSecureKeyForJwtTokenGenerationWithAtLeast32Characters",
    "ExpirationInDays": 7,
    "Issuer": "FrockIAM",
    "Audience": "FrockUsers"
  }
}
```

## Roles Disponibles

- `Traveller` (0) - Usuario viajero
- `TransportManager` (1) - Gestor de transporte

## Características Implementadas

- ✅ Autenticación JWT
- ✅ Hash seguro de contraseñas con BCrypt
- ✅ Middleware de autorización personalizado
- ✅ Validación de datos de entrada
- ✅ Manejo de errores robusto
- ✅ Documentación Swagger
- ✅ CORS configurado
- ✅ Arquitectura DDD (Domain Driven Design)
- ✅ Patrón Repository
- ✅ Unit of Work
- ✅ Servicios de Command y Query (CQRS)

## Base de Datos

Configurado para MySQL con Entity Framework Core.

## Middleware de Autorización

El middleware personalizado `RequestAuthorizationMiddleware` maneja:
- Verificación de tokens JWT
- Exclusión de endpoints con `[AllowAnonymous]`
- Inyección del usuario autenticado en el contexto HTTP

## Uso con Frontend

Para usar este microservicio desde un frontend:

1. Usar el endpoint `/sign-up` para registrar nuevos usuarios
2. Usar el endpoint `/sign-in` para autenticar usuarios y obtener JWT
3. Incluir el JWT en el header `Authorization: Bearer {token}` para endpoints protegidos
4. Usar `/validate-token` para verificar si un token sigue siendo válido

## Errores Resueltos

- ✅ Error de `System.ArgumentNullException: Value cannot be null. (Parameter 's')` en TokenService
- ✅ Configuración correcta de TokenSettings
- ✅ Referencias de using statements
- ✅ Implementación completa de todos los servicios y repositorios
- ✅ Middleware de autorización funcional
