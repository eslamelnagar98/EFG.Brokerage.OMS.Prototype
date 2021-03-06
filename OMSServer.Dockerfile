ARG Registery=""

# Stage 1 Build the solution
FROM ${Registery}mcr.microsoft.com/dotnet/framework/sdk AS Build

RUN mkdir /sources
WORKDIR /sources
COPY . .

RUN MSBuild EFG.OMSServer/EFG.OMSServer.csproj -t:build -restore -p:Configuration=Release

# Stage 2 build runtime image
FROM ${Registery}mcr.microsoft.com/dotnet/framework/runtime

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
RUN  Enable-WindowsOptionalFeature -Online -FeatureName WCF-TCP-Activation45, MSMQ-Server, MSMQ-Triggers, WCF-MSMQ-Activation45 -All

RUN mkdir app
WORKDIR /app
COPY --from=Build C:\\sources\\EFG.OMSServer\\bin\\Release\\ .
COPY  checkservice.ps1 .

RUN C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\InstallUtil.exe EFG.OMSServer.exe

EXPOSE 8097

CMD powershell -command ".\checkservice.ps1 Service1"
