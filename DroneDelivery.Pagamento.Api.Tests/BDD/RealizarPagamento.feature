#language: pt-br

Funcionalidade: RealizarPagamento
	Ao informar valores do cartao de credito
	Deve ser realizado pagamento do pedido


Esquema do Cenario: Receber valores do cartao de credito e pagar o pedido
	Dado Que eu tenha um pedido para pagamento receba um NumeroCartao: '<numeroCartao>' Vencimento: '<vencimento>' CodigoSeguranca: '<codigoSeguranca>'
	Quando eu solicitar a realização do pagamento
	Entao o pedido deverá ser pago a resposta devera ser um status code OK
	Exemplos: 
	| numeroCartao        | vencimento | codigoSeguranca |
	| 4242424242424242	  | 2020-12-01 | 123             |