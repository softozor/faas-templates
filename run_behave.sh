#! /bin/sh

OUTPUT_FILE=$1

python features/run_tc.py $OUTPUT_FILE || [ $? = 1 ]