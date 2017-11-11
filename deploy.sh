#!/bin/bash
DC_FILE=docker-compose.yml

sudo docker-compose -f $DC_FILE pull;
sudo docker-compose -f $DC_FILE down;
sudo docker-compose -f $DC_FILE up -d;