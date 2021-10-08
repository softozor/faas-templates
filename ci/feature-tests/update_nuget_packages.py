import argparse
import gitlab
import os
import re


def major_minor_patch(version):
    major, minor, patch = re.search('(\d+)\.(\d+)\.(\d+)', version).groups()
    return int(major), int(minor), int(patch)


def to_string_version(version_tuple):
    return '.'.join(tuple(str(x) for x in version_tuple))


def get_current_highest_version(packages, package_name):
    matching_packages = [package for package in packages if package.name == package_name]
    version = '1.0.0'
    for matching_package in matching_packages:
        version = max(major_minor_patch(version), major_minor_patch(matching_package.version))
        version = to_string_version(version)
    return version


def main(csproj, project_id):
    server_host = f"http://{os.environ['CI_SERVER_HOST']}"
    # doesn't work with CI_JOB_TOKEN
    private_token = os.environ['PRIVATE_ACCESS_TOKEN']

    package_name = 'Softozor.HasuraHandling'

    gl = gitlab.Gitlab(server_host, private_token=private_token)
    project = gl.projects.get(project_id)
    packages = project.packages.list(package_type='nuget')
    version = get_current_highest_version(packages, package_name)

    # 2. read features/examples/dotnet-hasura/DotNetHasura.csproj --> content

    # 3. replace current version with latest in content
    re.sub(r'(<PackageReference Include="Softozor.HasuraHandling" Version=")(\d.\d.\d(.\d)?)(" />)', r'\1 1.0.8\4',
           content)

    # 4. write result in features/examples/dotnet-hasura/DotNetHasura.csproj

if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--csproj', type=str, action='store')
    args = parser.parse_args()

    project_id = os.environ['CI_PROJECT_ID']

    main(args.csproj, project_id)