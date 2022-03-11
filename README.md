# Channel Demo

ChannelDemo is a .NET project that was created to learn more about .NET Channels and how they can be used to break work into smaller tasks that can be done concurrently.

The app will loop through JSON files, displaying a message per file of how many times an email address was in the logs. It will also show a grand total at the end.
## To use
* Clone repo, build solution.
* Have a directory with JSON files in the following format:
```json
{
  "id": "56f83bed-3705-4115-9067-73930cbecbc0",
  "logs": [
    {
      "id": "89004ef9-e825-4547-a83a-c9e9429e8f95",
      "email": "joe.dirt@email.com",
      "message": "A possible event message"
    }
  ]
}
```
* Run the solution giving it a the directory location you want it to read JSON data from.

## License
[MIT](https://choosealicense.com/licenses/mit/)