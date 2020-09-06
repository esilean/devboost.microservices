#language: pt-br

Funcionalidade: CriarPedidoParaPagamento
	Para realizar um pagamento no sistema
	Devemos receber o Id do Pedido e cadastrar na base


Cenario: Criar pedido para pagamento
	Dado Que eu receba um Id do Pedido e Valor
	Quando eu solicitar a criação do pedido
	Entao o pedido deverá ser cadastrado a resposta devera ser um status code OK
