#language: pt-br

Funcionalidade: ObterSituacaoDrone
	Para realizar uma consulta de status do drone
	Devemos consumir uma API


Cenario: Solicitar status dos drones
	Dado que existam drones
	E que este drone possua um pedido pago
	Quando eu solicitar o status dos drones
	Entao a resposta devera ser um statuscode OK e retornar os drones