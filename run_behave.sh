#! /bin/sh

python features/run_tc.py || [ $? = 1 ]