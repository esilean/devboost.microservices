#language: pt-br

Funcionalidade: ObterSituacaoDrone
	Para realizar uma consutla de status do drone
	Devemos consumir uma API


Cenario: Solicitar status do drone
	Dado que exista um drone 
	E que este drone possua um pedido
	Quando eu solicitar o status do drone
	Entao a resposta devera ser um status code 200OK
	E devera retornar os pedidos do drone