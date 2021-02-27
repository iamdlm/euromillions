## Useful links
- https://nunofcguerreiro.com/blog/euro-milhoes-api
- [Configuration in .NET Core console applications](https://blog.hildenco.com/2020/05/configuration-in-net-core-console.html)
- [Console app with configuration, dependency injection and logging](https://emanuelpaul.net/2019/06/03/console-app-with-configuration-dependency-injection-and-logging/)
- [Adding Dependency Injection to .NET Core Console Applications on Windows](https://dev.to/ballcapz/adding-dependency-injection-to-net-core-console-applications-on-windows-3pm0)

## Usage
1. Add smtp settings to appsettings.json
2. Schedule task with Windows Task Scheduler (eg: Run every Tuesday and Friday at 10 am)

## How it works
- Removes keys with previous big prizes won (usually 5th or better)
- Removes keys already drawn
- Removes keys outside of range between previous draws numbers sum average +/- standard deviation
- Removes all patterns not equal to 3-odd-2-even or 3-even-2-odd 
- Removes all patterns not equal to 3-low-2-high or 2-low-3-high (low = {1,...,25}, high = {26,...,50})
- Removes sequential keys

## To do
- [ ] Strongly-typed configuration
- [ ] Track generated keys results
