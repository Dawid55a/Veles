# Veles
Final project for Programming III. In the form of a multiplayer chat.

# Information for developer

In case docker image is not building property after changes in image just purge everything:
```
Remove all docker containers: docker rm -f $(docker ps -a -q)
Remove all docker images:     docker rmi -f $(docker images)
```
How to run docker file for server? 
In main Veles folder run:
```
docker build -f VelesServer/Dockerfile . -t veles_server
```