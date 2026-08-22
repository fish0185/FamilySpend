install .net 10 sdk
 
// install tool
dotnet tool install --global dotnet-ef

// mac
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zshrc
source ~/.zshrc

dotnet ef migrations add init -o Persistence/Migrations

dotnet ef database update