#! /bin/sh

OUTPUT_FILE=$1

python features/run_tc.py --output-file $OUTPUT_FILE || [ $? = 1 ]