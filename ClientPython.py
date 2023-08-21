import requests
import time
import random

def create_comment(comment_text):
    response = requests.post("http://localhost:5230/api/comments", json=comment_text, verify=False)
    return response.text

def get_comment_status(request_id):
    response = requests.get(f"http://localhost:5230/api/comments/{request_id}")
    return response.status_code

def main():
    comments = ["Great post!", "Thanks for sharing.", "Interesting topic."]
    
    for comment_text in comments:
        request_id = create_comment(comment_text)
        print(f"Comment '{comment_text}' sent. Request ID: {request_id}")
    
    while comments:
        comment_text = comments.pop(0)
        request_id = create_comment(comment_text)
        print(f"Comment '{comment_text}' sent. Request ID: {request_id}")
        time.sleep(1)

    print("Waiting for comments to be processed...")
    while comments:
        request_id = comments[0]
        status_code = get_comment_status(request_id)
        if status_code == 202:
            print(f"Comment with Request ID {request_id} has been processed.")
            comments.pop(0)
        else:
            print(f"Comment with Request ID {request_id} is still pending.")
        time.sleep(1)

if __name__ == "__main__":
    main()