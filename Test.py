from flask import Flask, request
from urllib.parse import parse_qs
import struct
import json
import pdb

app = Flask(__name__)
global_description = "";

@app.route("/")
def home():
	return "Hello! This is the main page <h1>HELLO<h1>"

@app.route("/<name>")
def user(name):
		return f"Hello {name}!"

@app.route("/Playback/clientPBState", methods=["POST","GET","PUT"])
def play_pause():
	if request.method == "PUT":
		print("Start processing put request for play and pause");
		client_data = request.get_data();
		decoded_data = struct.unpack('ii', client_data);
		#decode_client_data = client_data.decode('utf-8');
		#decode_string = json.loads(decode_client_data);
		print(decoded_data);
		print(client_data);
		return client_data;
	else:
		return "Successful!"

@app.route("/Playback/seek", methods=["POST","GET","PUT"])
def seek_time():
	if request.method == "PUT":
		print("Start processing put request for seeking time");
		time_stamp = request.get_data();
		decoded_time_stamp = struct.unpack('<d', time_stamp)[0];
		print(time_stamp);
		print(decoded_time_stamp);
		return time_stamp;
	else:
		return "Successful!"

@app.route("/Playback/seekframe", methods=["POST","GET","PUT"])
def seek_frame():
	if request.method == "PUT":
		print("Start processing put request");
		json = request.get_json();
		client_id = (int)(json["clientID"]);
		client_state = (int)(json["clientState"]);
		print("Received put request:" + (str)(client_id) + "," + (str)(client_state));
		if client_state == 0:
			print("Start live streaming!")
		else:
			print("Stop live streaming!")
		return (str)(client_id) + "," + (str)(client_state);
	else:
		return "Successful!"

if __name__ == "__main__":
	app.run(port=5001)