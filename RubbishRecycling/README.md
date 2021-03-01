# InfoTrack.Cdd (Customer Due Diligence)
_CDD/KYC/KYB/AML microservice_

**"CDD"** is the process of identifying customers and checking they are who they say they are. This business domain encompasses KYC/KYB and AML.

This microservice will initially provide an alternative/replacement to KYCIT international company searches (formerly provided by BvD). In the near future AML personal searches will also be added (replacing Dow Jones' current offering).

Later on, more searches and/or providers may also be added.

## Frontend Docs ⬇

Please refer to [this readme 😀](https://github.com/InfoTrackGlobal/InfoTrack.Cdd/tree/develop/client)

## API

### API solution overview
This solution was built from the [Global Platform API Template](https://github.com/InfoTrackGlobal/InfoTrack.GlobalPlatform.Template), which uses Mediatr and CQRS patterns. Refer to [this readme](https://github.com/InfoTrackGlobal/InfoTrack.GlobalPlatform.Template) for general notes on this kind of application structure.

### Links
#### AU
| Env | API | UI |
|---|---|---|
| [Test](https://testsearch.infotrack.com.au/service/cdd/api/swagger/index.html) | https://testsearch.infotrack.com.au/service/cdd/api/swagger/index.html | https://testsearch.infotrack.com.au/service/cdd |
| [Stage](https://stagesearch.infotrack.com.au/service/cdd/api/swagger/index.html) | https://stagesearch.infotrack.com.au/service/cdd/api/swagger/index.html | https://stagesearch.infotrack.com.au/service/cdd |
| [Prod](https://search.infotrack.com.au/service/cdd/api/swagger/index.html) | https://search.infotrack.com.au/service/cdd/api/swagger/index.html | https://search.infotrack.com.au/service/cdd |

TeamCity: http://ci.infotrack.com.au/project.html?projectId=DockerProjects_InfoTrackCdd

Octopus: http://ci.infotrack.com.au:88/app#/Spaces-1/projects?projectGroupId=ProjectGroups-408

Rancher: https://rancher.infotrack.com.au/env/1a2243/apps/stacks/1st971

#### UK
| Env | API | UI |
|---|---|---|
| [Test](https://testsearch.infotrack.co.uk/service/cdd/api/swagger/index.html) | https://testsearch.infotrack.co.uk/service/cdd/api/swagger/index.html <sub>UK Test is only accessible from within UK local VPN.</sub> | https://testsearch.infotrack.co.uk/service/cdd |
| [Stage](https://stagesearch.infotrack.co.uk/service/cdd/api/swagger/index.html) | https://stagesearch.infotrack.co.uk/service/cdd/api/swagger/index.html | https://search.infotrack.co.uk/service/cdd/api/swagger/index.html |
| [Prod](https://search.infotrack.co.uk/service/cdd/api/swagger/index.html) | https://search.infotrack.co.uk/service/cdd/api/swagger/index.html | https://search.infotrack.co.uk/service/cdd |

TeamCity: *Managed by AU*

Octopus: *Managed by AU*

Rancher: https://rancher.infotrack.co.uk/env/1a109887/apps/stacks/1st286

#### US
| Env | API | UI |
|---|---|---|
| [Test](https://testsearch.infotrack.com/service/cdd/api/swagger/index.html) | https://testsearch.infotrack.com/service/cdd/api/swagger/index.html | https://testsearch.infotrack.com/service/cdd |
| [Stage](https://stagesearch.infotrack.com/service/cdd/api/swagger/index.html) | https://stagesearch.infotrack.com/service/cdd/api/swagger/index.html | https://stagesearch.infotrack.com/service/cdd |
| [Prod](https://search.infotrack.com/service/cdd/api/swagger/index.html) | https://search.infotrack.com/service/cdd/api/swagger/index.html | https://search.infotrack.com/service/cdd |

TeamCity: *Managed by AU* 

Octopus: *Managed by AU* 

Rancher: https://rancher.infotrack.com/env/1a34/apps/stacks/1st149

### Developing locally

To run locally for the first time, just do the obvious:
1. Load `InfoTrack.Cdd.sln` in Visual Studio
2. Right-click `InfoTrack.Cdd.Api` project and select 'Set as Startup Project'
3. Select which environment to run against, by using the little drop-down arrow next to the green Run/Play button (default should be Dev). These environment configurations are managed via `launchSettings.json`.
4. Click **Run/Play**

### Testing

The API solution contains two test projects:

#### `InfoTrack.Cdd.Application.UnitTests`
This project contains unit tests for the Application and Infrastructure layers. Currently, these tests are executed in the TeamCity build script. Test results are not being report back to TeamCity, but a failing unit test will break the build and prevent Octopus deployment. 

Tests should be executed locally prior to pushing changes, to avoid unexpected build failures.

##### Helpful hints:
When developing or making changes to report templates, `TemplateBuilderTests.cs` are extremely helpful. The RazorLight cshtml template can be built and saved to your local machine within seconds by running a single test.

#### `InfoTrack.Cdd.Api.IntegrationTests`
These are integration tests executed on the API layer. They can be used for testing end-to-end flow. These tests may not currently be part of the TeamCity build script.

##### Helpful hints: 
When running integration tests, mocks are used for all external (and some internal) services. These mocks are added to the ServiceCollection from `CustomWebApplicationFactory.cs` `AddTestServices`. For local testing, it is possible to comment out any/all of these service injections, and that will result in the real version of that service being called (using test configs). E.g. a test order can be created in LDM and the Frankies test endpoint can be called to add a test-data report to that order. This enables extensive end-to-end testing without the need to run or log into the UI.

### Bugfixing

The quality and quantity of international company data is inconsistent, because each country uses different primary data source/s (e.g. Australian companies are registered with ASIC whereas UK companies are registered with Companies House). 

For this reason, it is highly likely that bugs will be found after release that weren't detected during development and testing, particularly when reports are ordered for outlying countries. Testing and fixing these kinds of bugs should be easy:

#### 1. Unexpected data format errors, poorly formatted reports, missing data, serialisation errors, etc
For every order, the raw authority response is stored in its original `.json` format in MongoDB (`Orders` db, `responses` collection). Unit tests can be run directly against this data to find bugs and generate report templates locally (refer to testing instructions above).
