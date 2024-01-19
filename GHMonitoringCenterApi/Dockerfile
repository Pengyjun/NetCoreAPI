#FROM net5libgdiplus:latest
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
##ÉèÖÃÊ±Çø
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "GHMonitoringCenterApi.dll"]
