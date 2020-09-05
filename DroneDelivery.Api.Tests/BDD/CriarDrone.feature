#language: pt-br

Funcionalidade: CriarDrone
	Para criar um drone no sistema
	Devemos consumir uma API


Esquema do Cenario: Criar drone
	Dado que o cliente esteja logado
	Quando eu solicitar a criacao de drones com os detalhes  Capacidade: '<capacidade>' Velocidade: '<velocidade>' Autonomia: '<autonomia>' Carga: '<carga>'
	Entao a resposta devera ser um status code 200OK
	Exemplos: 
	| capacidade | velocidade | autonomia | carga |
	| 12000      | 3.3333     | 35        | 100   |
	| 5000       | 3.3333     | 10        | 100   |
	| 1000       | 2.0        | 20        | 100   |
	| 10000      | 3.3333     | 35        | 30    |

Esquema do Cenario: Criar drone com dados invalidos
	Dado que o cliente esteja logado
	Quando eu solicitar a criacao de drones com os detalhes  Capacidade: '<capacidade>' Velocidade: '<velocidade>' Autonomia: '<autonomia>' Carga: '<carga>'
	Entao a resposta devera ser um status code 400BadRequest
	Exemplos: 
	| capacidade | velocidade | autonomia | carga |
	| 0          | 3.3333     | 35        | 100   |
	| 5000       | 0          | 10        | 100   |
	| 1000       | 2.0        | 0         | 100   |
	| 10000      | 3.3333     | 35        | 0     |
