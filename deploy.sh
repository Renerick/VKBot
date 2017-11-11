#!/bin/bash
DIR=~/docker
DC_FILE=docker-compose.yml

cd $DIR;

docker-compose -f $DC_FILE pull;
docker-compose -f $DC_FILE down;
docker-compose -f $DC_FILE up -d;