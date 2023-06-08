up: build host

build: 
	dotnet restore ./src
	dotnet build ./src
	
host: 
	docker-compose up

clean: 
	dotnet clean ./src
	docker-compose down