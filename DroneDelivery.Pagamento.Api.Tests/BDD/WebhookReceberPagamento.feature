#language: pt-br

Funcionalidade: WebHookReceberPagamento
	Receber a atualização dos pagamentos pelo webhook
	O pagamento deve ter o status aprovado ou reprovado


Esquema do Cenario: Receber valores do do gateway de pagamento através do web hook
	Dado Que eu tenha um pedido que foi pago e esteja aguardando aprovação 
	E o webhook me retornar a situação deste pedido com status StatusPagamento: '<statusPagamento>'
	Quando eu receber esta atualização
	Entao o pedido deverá ser aprovado ou reprovado
	Exemplos: 
	| statusPagamento   |
	| Aprovado			|
	| Reprovado			|