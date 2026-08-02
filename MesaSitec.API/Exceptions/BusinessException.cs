namespace MesaSitec.API.Exceptions;

public class BusinessException : Exception
{
    public int StatusCode { get; }

    public string Codigo { get; }

    public string Title { get; }

    public IDictionary<string, string[]>? Errors { get; }

    public BusinessException(
        int statusCode,
        string codigo,
        string title,
        string detail,
        IDictionary<string, string[]>? errors = null)
        : base(detail)
    {
        StatusCode = statusCode;
        Codigo = codigo;
        Title = title;
        Errors = errors;
    }
}