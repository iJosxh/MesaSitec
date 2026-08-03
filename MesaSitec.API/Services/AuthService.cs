using MesaSitec.API.Data;
using MesaSitec.API.DTOs;
using MesaSitec.API.Helpers;
using MesaSitec.API.Models;
using MesaSitec.API.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MesaSitec.API.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenGenerator _jwt;

    public AuthService(
        ApplicationDbContext context,
        JwtTokenGenerator jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
{
    var usuario = await _context.Usuarios
        .Include(u => u.Tenant)
        .FirstOrDefaultAsync(u => u.Email == request.Email);

    if (usuario is null || !usuario.Activo)
    {
        throw new BusinessException(
            StatusCodes.Status401Unauthorized,
            "NO_AUTENTICADO",
            "No autenticado",
            "Las credenciales son incorrectas.");
    }

    var passwordHasher = new PasswordHasher<Usuario>();

    var resultado = passwordHasher.VerifyHashedPassword(
        usuario,
        usuario.PasswordHash,
        request.Password);

    if (resultado == PasswordVerificationResult.Failed)
    {
        throw new BusinessException(
            StatusCodes.Status401Unauthorized,
            "NO_AUTENTICADO",
            "No autenticado",
            "Las credenciales son incorrectas.");
    }

    var token = _jwt.GenerateToken(usuario);

    return new LoginResponse
    {
        AccessToken = token,
        ExpiraEn = 28800,
        Usuario = new UsuarioResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.ToString(),
            TenantId = usuario.TenantId,
            TenantNombre = usuario.Tenant!.Nombre
        }
    };
}
}