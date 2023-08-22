import requests
import time
import json
import urllib3
from uuid import UUID

def create_comment(client, text):
    response = client.post("https://localhost:7234/api/comment", json={"Text": text})
    return response.json()

def get_comment(client, request_id):
    response = client.get(f"https://localhost:7234/api/comment/{request_id}")
    if response.status_code == 200:
        return response.json()
    return None

def main():
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
    client = requests.Session()
    client.verify = False
    requests_id_list = []

    for i in range(10):
        comment = f"Comment {i + 1}"
        request_id = create_comment(client, comment)
        requests_id_list.append(request_id)
        print(f"Comment '{comment}' sent with request ID: {request_id}")

    while True:
        if requests_id_list.__len__() == 0:
            break

        comment_data = None
        for id in requests_id_list:
            comment_data = get_comment(client, id)
            if comment_data:
                print(f"Received comment: {comment_data.get('text', 'N/A')}\n\tID: {request_id}\n\ttime post: {comment_data.get('datePost', 'N/A')}\n\ttime added: {comment_data.get('dateAdded', 'N/A')}")
                requests_id_list.remove(id)
            
        time.sleep(2)

if __name__ == "__main__":
    main()
