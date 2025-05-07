# C# WebApi using EntityFramework

How to install using docker ?
```
docker build -f WebApi/Dockerfile -t webapi-sqlite .
docker run --rm -it -p 5550:8080 webapi-sqlite
```
After install, you can access swagger in http://localhost:5550/swagger/index.html
