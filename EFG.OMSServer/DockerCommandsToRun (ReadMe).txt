docker image build --no-cache -t omsserverimage:latest -f .\EFG.OMSServer.dockerfile .
docker run -d -p 8079:8079 omsserverimage:latest