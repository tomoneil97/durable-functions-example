up: host

host: 
	docker-compose up

down: clean

clean: 
	dotnet clean ./src
	docker-compose down --rmi all