#!/bin/bash

echo "cleaning all helm releases from cluster"
helm ls --short | xargs -L1 helm delete --purge
