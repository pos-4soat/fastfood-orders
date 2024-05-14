namespace fastfood_orders.Domain.Contants
{
    public static class ErrorMessages
    {
        public static Dictionary<string, string> ErrorMessageList => _errorMessages;

        private static readonly Dictionary<string, string> _errorMessages = new()
        {
            { "OBE001", "Request inválido ou fora de especificação, vide documentação." },
            { "OBE002", "O nome deve estar preenchido." },
            { "OBE003", "O nome deve ter no minimo 3 e no máximo 255 caracteres." },
            { "OBE004", "Informar produtos válidos para geração do pedido." },
            { "OBE005", "O pedido precisa ter pelo menos um produto para ser criado." },
            { "OBE006", "O id do produto precisa estar preenchido." },
            { "OBE007", "A quantidade do produto precisa ser maior que zero." },
            { "OBE008", "O CPF do usuário não pode estar vazio quando fornecido." },
            { "OBE009", "O CPF do usuário precisa ser válido quando fornecido." },
            { "OBE010", "Nenhum pedido encontrado com os parâmetros informados." },
            { "OBE011", "O status do pedido é inválido." },
            { "OBE012", "O id pedido precisa ser maior que zero." },
            { "OBE013", "O status do pedido é inválido." },
            { "OBE014", "Novo status do pedido inválido." },
            { "OIE999", "Internal server error" }
        };
    }
}
