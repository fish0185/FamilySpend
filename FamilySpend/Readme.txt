install .net 10 sdk
 
// install tool
dotnet tool install --global dotnet-ef

// mac
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zshrc
source ~/.zshrc

dotnet ef migrations add update3 --context FamilySpendDbContext  -o Persistence/Migrations

dotnet ef database update --context FamilySpendDbContext

docker build -t fish0185/familyspend -f Dockerfile .                

docker network create my-custom-network

docker run -d -p 8000:8080 --name fs --network my-custom-network fish0185/familyspend 

docker run -d --name ps -e POSTGRES_PASSWORD=postgres --network my-custom-network -p 55432:5432 postgres:15.0

