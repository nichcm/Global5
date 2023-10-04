namespace Global5.Domain.Constants
{
    public class ApplicationMessages
    {
        public const string RESPONSE_ERROR = "Erro ocorrido na requisição.";
        public const string VALIDATIONS_TITLE = "Problema(s) com a(s) validação(ões) do request, verifique a(s) mensagem(s) de Abaixo:";
        public const string NULLREQUEST = "Request está nulo.";

        public const string GetMessageRecoveryPass = "<p>Ol&aacute;, <strong>@Name</strong>!<strong><br /><br /></strong>Recebemos um aviso informando que voc&ecirc; <strong>Esqueceu a sua senha</strong>.</p>" +
                                                        "</p><p>Voc&ecirc; deve informar o<span style=text-decoration: underline;> Token</span>:&nbsp;<strong>@Token</strong> para fazer a mudan&ccedil;a de senha.</p>" +
                                                        "<p>Se tiver alguma d&uacute;vida, nos envie um e-mail.<br /><br />Obrigado,<strong><br />Time @Time</strong></p>";
    }
}