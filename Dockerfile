FROM mcr.microsoft.com/dotnet/framework/sdk AS Build

RUN mkdir /sources
WORKDIR /sources
COPY . .

RUN MSBuild EFG.Brokerage.OMS.Prototype.sln /P:Configuration=Release

FROM mcr.microsoft.com/dotnet/framework/runtime

RUN  PowerShell -Command "Enable-WindowsOptionalFeature -Online -FeatureName WCF-TCP-Activation45, MSMQ-Server, MSMQ-Triggers, WCF-MSMQ-Activation45 -All"

RUN mkdir orderservice
WORKDIR /orderservice
COPY --from=Build C:\\sources\\EFG.OrderService.WindowsServiceHoster\\bin\\Release\\ .

RUN C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\InstallUtil.exe EFG.OrderService.WindowsServiceHoster.exe
