#!/bin/bash
DIR=~/docker
DC_FILE=docker-compose.yml

cd $DIR;

sudo docker-compose -f $DC_FILE pull;
sudo docker-compose -f $DC_FILE down;
sudo docker-compose -f $DC_FILE up -d;