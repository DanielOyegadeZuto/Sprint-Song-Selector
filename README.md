Song selector application Usage Documentation
Overview
This documentation provides a simple guide on how to use the Song Selector API. The API allows you to retrieve a list of songs from a specified artist, filtered by a starting letter. You can easily interact with the API using the Swagger UI provided.
Steps to Use the Application
1.	Clone the Repository:
o	Clone the repository to your local machine using the following command:
git clone < https://github.com/DanielOyegadeZuto/Sprint-Song-Selector.git>

3.	Pull Down Any Changes:
•	Navigate to the cloned directory and pull down any changes:
•	Use the command:
 git pull

5.	Run the Application:
Start the application by navigating to the project directory and running the following command:
dotnet run

7.	Access Swagger UI:
•	Once the application is running, open your web browser and navigate to the Swagger UI. Typically, it will be available at:
http://localhost:<port>/swagger

•	Replace <port> with the port number where your application is running (usually displayed in the console when the app starts).

5.	Using the Get Artist Method:
o	In the Swagger UI, locate the method named getArtistSongs under the SongSelector controller.
o	This method allows you to retrieve songs from a specific artist, filtered by the starting letter.
Required Fields
•	artistId: The unique ID of the artist. This is a required field.
•	totalLimit: The maximum number of songs to retrieve from the artist's catalogue. This is a required field.
•	letter: The starting letter to filter the songs by.
Example Artist IDs
To simplify the process, you can use the following example artist IDs:
•	Bloc Party: 3MM8mtgFzaEJsqbjZBSsHJ
•	Pink Floyd: 0k17h0D3J5VfsdmQ1iZtE9
•	King Krule: 4wyNyxs74Ux8UIDopNjIai
•	The Beatles: 3WrFJ7ztbogyGnTHbHJFl2

Example Request

1.	Input Parameters:
o	artistId: 0k17h0D3J5VfsdmQ1iZtE9 (Pink Floyd)
o	totalLimit: 300
o	letter: A

2.	Steps:
o	Enter the artistId in the artistId field.
o	Enter 100 in the totalLimit field to retrieve up to 100 songs.
o	Enter A in the letter field to filter songs starting with the letter 'A'.

3.	Submit the Request:
o	Click the "Execute" button to submit the request.
o	The response will display a list of songs from the specified artist, filtered by the provided letter if applicable.


Output
The output will be a JSON object containing a list of songs matching the criteria. For example:






