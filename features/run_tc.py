import os
import sys
import traceback
import argparse

from behave.configuration import Configuration
from behave.formatter import _registry
from behave.formatter.base import StreamOpener
from behave.runner import Runner
from behave_teamcity import TeamcityFormatter
from behave_to_cucumber_json_formatter import PrettyCucumberJSONFormatter


here = os.path.dirname(os.path.abspath(__file__))

_registry.register_as("TeamcityFormatter", TeamcityFormatter)
_registry.register_as("PrettyCucumberJSONFormatter", PrettyCucumberJSONFormatter)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("output_file", type=str)
    args = parser.parse_args()

    # TODO: activate when we use behave 1.2.7
    # tags = ["~@wip"]
    tags = []

    configuration = Configuration(tags=tags)
    configuration.format = ["PrettyCucumberJSONFormatter", "TeamcityFormatter"]
    configuration.stdout_capture = False
    configuration.stderr_capture = False
    configuration.paths = [here]
    configuration.outputs = [StreamOpener(args.output_file)]

    runner = Runner(configuration)

    try:
        success = runner.run()
        print(f"Test runner returned: {success}")
        if runner.hook_failures > 0:
            print(f"Encountered {runner.hook_failures} hook failures", file=sys.stderr)
            sys.exit(2)
    except:  # noqa: E722
        print("Encountered unhandled error", file=sys.stderr)
        traceback.print_exception(*sys.exc_info(), file=sys.stderr)
        sys.exit(2)

    sys.exit(int(success))


if __name__ == "__main__":
    main()
