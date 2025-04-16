
"""
Game Store Console Client!

this is a little Python app that talks to our ASP.NET GameStore API.
we can run it from the terminal to:
- view/add/update/delete games
- view/add/update/delete users
- place and manage orders
- search for users and games
- view/add/delete rewards (user points)
- uses BasicAuth for security

make sure the API is running locally (https://localhost:7200), and the following is installed:
    the pip requirements.txt inside terminal using:
        pip install -r requirements.txt
"""

import requests
import json
import urllib3
import os
from requests.auth import HTTPBasicAuth

#to shut up the SSL warnings for localhost
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

#hardcoded credentials
AUTH = HTTPBasicAuth("admin", "password")

BASE_URL = "https://localhost:7200/api"

#splash screen
def show_splash():
    print(r"""
 _   _      _ _        
| | | | ___| | | ___   
| |_| |/ _ \ | |/ _ \  
|  _  |  __/ | | (_) | 
|_| |_|\___|_|_|\___/  

      Welcome to Retro Games Store Console Client!
""")

def print_menu():
    print("\n=== Game Store Console Client ===")
    
    print("📚 Games")
    print("  1. View all games")
    print("  2. Add a new game")
    print("  3. View a game by ID")
    print("  4. Delete a game")
    print("  5. Search games by name")
    print("  6. Update a game")

    print("\n👥 Users")
    print("  7. View all users")
    print("  8. Add a new user")
    print("  9. View a user by ID")
    print(" 10. Delete a user")
    print(" 11. Search users by name/email")
    print(" 12. Update a user")

    print("\n🧾 Orders")
    print(" 13. View all orders")
    print(" 14. Place a new order")
    print(" 15. View an order by ID")
    print(" 16. Delete an order")
    print(" 17. Update an order")

    print("\n🎁 Rewards")
    print(" 18. View all rewards")
    print(" 19. Add new reward")
    print(" 20. View a reward by ID")
    print(" 21. Delete a reward")

    print("\n🚪 0. Exit")
    print("=================================")

#1. GET all games
def view_all_games():
    url = f"{BASE_URL}/Games"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        games = response.json()
        print("\nAvailable Games:")
        for game in games:
            print(f" - {game['id']}: {game['name']} | {game['genre']} | ${game['price']}")
    else:
        print("Failed to fetch games ☹")

    input("\nPress Enter to return to the menu...")


#2. POST a new game
def add_new_game():
    print("\nEnter new game details:")
    name = input("Name: ")
    genre = input("Genre: ")
    platform = input("Platform: ")
    description = input("Description: ")
    price = float(input("Price: "))
    stock = int(input("Stock: "))
    release_date = input("Release Date (YYYY-MM-DD): ")

    payload = {
        "name": name,
        "genre": genre,
        "platform": platform,
        "description": description,
        "price": price,
        "stock": stock,
        "releaseDate": release_date
    }

    response = requests.post(f"{BASE_URL}/Games", json=payload, verify=False)

    if response.status_code == 201:
        print("Game added successfully! ヅ")
    else:
        print("Error adding game ☹:", response.text)
          
    input("\nPress Enter to return to the menu...")

#3. GET all users
def view_all_users():
    url = f"{BASE_URL}/Users"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        users = response.json()
        print("\nRegistered Users:")
        for user in users:
            print(f" - {user['userId']}: {user['userName']} | {user['email']}")
    else:
        print("Failed to fetch users ☹.")

    input("\nPress Enter to return to the menu...")

#4. POST a new user
def add_new_user():
    print("\nEnter new user details:")
    userName = input("Username: ")
    password = input("Password: ")
    email = input("Email: ")
    dob = input("Date of Birth (YYYY-MM-DD): ")
    address = input("Address: ")
    city = input("City: ")
    state = input("State: ")
    zipCode = input("Zip Code: ")
    country = input("Country: ")

    payload = {
        "userName": userName,
        "password": password,
        "email": email,
        "dateOfBirth": dob,
        "address": address,
        "city": city,
        "state": state,
        "zipCode": zipCode,
        "country": country
    }

    response = requests.post(f"{BASE_URL}/Users", json=payload, verify=False)

    if response.status_code == 201:
        print("User added successfully! ヅ")
    else:
        print("Error adding user ☹:", response.text)

    input("\nPress Enter to return to the menu...")

#5. GET all orders
def view_all_orders():
    url = f"{BASE_URL}/Orders"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        orders = response.json()
        print("\nOrders List:")
        for order in orders:
            print(f" - Order #{order['orderId']}: User {order['userId']} bought Game {order['gameId']} x{order['quantity']} on {order['orderDate']}")
    else:
        print("Failed to fetch orders ☹.")
    
    input("\nPress Enter to return to the menu...")

#6. POST a new order to the API
def place_order():
    print("\n🛒 Place a New Order")
    print("Listing users and games so you can pick valid IDs...")

    view_all_users()   #quick look at available users
    view_all_games()   #quick look at games

    #ask user for all the info we need
    try:
        user_id = int(input("Enter User ID: "))
        game_id = int(input("Enter Game ID: "))
        quantity = int(input("How many copies?: "))
        date = input("Order Date (YYYY-MM-DD): ")
        amount = float(input("Total amount ($): "))

        #optional: future proofing for when we handle status or shipping flags
        status = input("Order Status (optional, e.g., 'Pending'): ").strip()
        payment = input("Payment Method (optional): ").strip()
        shipped = input("Is it shipped yet? (y/n): ").strip().lower()
        is_shipped = shipped == "y"

        #optional fields default to None if not provided
        status = status if status else None
        payment = payment if payment else None

        #create the payload to send to the API
        payload = {
            "userId": user_id,
            "gameId": game_id,
            "quantity": quantity,
            "orderDate": date,
            "totalAmount": amount,
            "status": status,
            "paymentMethod": payment,
            "isShipped": is_shipped
        }

        #send POST request to API
        response = requests.post(f"{BASE_URL}/Orders", json=payload, verify=False)

        #check result
        if response.status_code == 201:
            print("Order placed successfully! ヅ")
        else:
            try:
                print("Failed to place order ☹. Details:", response.json())
            except:
                print("Failed to place order ☹. Raw error:", response.text)

    except ValueError:
        print("Invalid input ☹. Make sure IDs and amounts are numbers.")
    except Exception as e:
        print("Something went wrong ☹:", e)
    
    input("\nPress Enter to return to the menu...")

#7. GET a single game by ID
def get_game_by_id():
    game_id = input("\nEnter Game ID to view: ")
    url = f"{BASE_URL}/Games/{game_id}"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        game = response.json()
        print(f"\n🎮 Game #{game['id']}:")
        print(f" - Name: {game['name']}")
        print(f" - Genre: {game['genre']}")
        print(f" - Platform: {game['platform']}")
        print(f" - Price: ${game['price']}")
        print(f" - Stock: {game['stock']}")
        print(f" - Released: {game['releaseDate']}")
    else:
        print("Game not found, maybe soon ☹.")

    input("\nPress Enter to return to the menu...")

#8. GET a single user by ID
def get_user_by_id():
    user_id = input("\nEnter User ID to view: ")
    url = f"{BASE_URL}/Users/{user_id}"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        user = response.json()
        print(f"\n👤 User #{user['userId']}:")
        print(f" - Name: {user['userName']}")
        print(f" - Email: {user['email']}")
        print(f" - DOB: {user['dateOfBirth']}")
        print(f" - Address: {user['address']}, {user['city']}, {user['state']}, {user['zipCode']}, {user['country']}")
    else:
        print("User not found ☹.")

    input("\nPress Enter to return to the menu...")

#9. GET a single order by ID
def get_order_by_id():
    order_id = input("\nEnter Order ID to view: ")
    url = f"{BASE_URL}/Orders/{order_id}"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        order = response.json()
        print(f"\n📦 Order #{order['orderId']}:")
        print(f" - User: {order['userId']}")
        print(f" - Game: {order['gameId']}")
        print(f" - Quantity: {order['quantity']}")
        print(f" - Total: ${order['totalAmount']}")
        print(f" - Date: {order['orderDate']}")
        print(f" - Status: {order['status']}")
        print(f" - Payment: {order['paymentMethod']}")
        print(f" - Shipped: {'Yes' if order['isShipped'] else 'No'}")
    else:
        print("Order not found ☹.")

        input("\nPress Enter to return to the menu...")

#10. DELETE a game by ID
def delete_game():
    game_id = input("\nEnter Game ID to delete: ")
    confirm = input(f"Are you SURE you want to delete Game #{game_id}? (y/n): ").lower()
    if confirm == "y":
        response = requests.delete(f"{BASE_URL}/Games/{game_id}", verify=False)
        if response.status_code == 204:
            print("Game deleted ヅ.")
        else:
            print("Failed to delete game ☹.")
    else:
        print("Deletion canceled.")
    
    input("\nPress Enter to return to the menu...")

#11 DELETE a user by ID
def delete_user():
    user_id = input("\nEnter User ID to delete: ")
    confirm = input(f"Are you sure you want to delete User #{user_id}? (y/n): ").lower()
    if confirm == "y":
        response = requests.delete(f"{BASE_URL}/Users/{user_id}", verify=False)
        if response.status_code == 204:
            print("User deleted ヅ.")
        else:
            print("Failed to delete user ☹.")
    else:
        print("Deletion canceled.")
    
    input("\nPress Enter to return to the menu...")

#12 DELETE an order by ID
def delete_order():
    order_id = input("\nEnter Order ID to delete: ")
    confirm = input(f"Are you sure you want to delete Order #{order_id}? (y/n): ").lower()
    if confirm == "y":
        response = requests.delete(f"{BASE_URL}/Orders/{order_id}", verify=False)
        if response.status_code == 204:
            print("Order deleted ヅ.")
        else:
            print("Failed to delete order ☹.")
    else:
        print("Deletion canceled.")

    input("\nPress Enter to return to the menu...")

#13 search games by name
def search_games_by_name():
    query = input("\nEnter game name to search: ").lower()
    response = requests.get(f"{BASE_URL}/Games", verify=False)

    if response.status_code == 200:
        games = response.json()
        matches = [g for g in games if query in g['name'].lower()]
        if matches:
            print("\nMatching Games:")
            for game in matches:
                print(f" - {game['id']}: {game['name']} | {game['genre']} | ${game['price']}")
        else:
            print("No matching games found ☹.")
    else:
        print("Error fetching games ☹.")

    input("\nPress Enter to return to the menu...")

#14. search users by name or email
def search_users():
    query = input("\nEnter name or email to search: ").lower()
    response = requests.get(f"{BASE_URL}/Users", verify=False)

    if response.status_code == 200:
        users = response.json()
        matches = [u for u in users if query in u['userName'].lower() or query in u['email'].lower()]
        if matches:
            print("\nMatching Users:")
            for user in matches:
                print(f" - {user['userId']}: {user['userName']} | {user['email']}")
        else:
            print("No users found matching that ☹.")
    else:
        print("Error fetching users ☹.")

    input("\nPress Enter to return to the menu...")

#15. update game by ID
def update_game():
    game_id = input("\nEnter Game ID to update: ")
    print("Leave fields blank to keep existing values.")
    
    response = requests.get(f"{BASE_URL}/Games/{game_id}", verify=False)
    if response.status_code != 200:
        print("Game not found ☹.")
        return

    game = response.json()
    game['name'] = input(f"Name [{game['name']}]: ") or game['name']
    game['genre'] = input(f"Genre [{game['genre']}]: ") or game['genre']
    game['platform'] = input(f"Platform [{game['platform']}]: ") or game['platform']
    game['description'] = input(f"Description [{game['description']}]: ") or game['description']
    game['price'] = float(input(f"Price [{game['price']}]: ") or game['price'])
    game['stock'] = int(input(f"Stock [{game['stock']}]: ") or game['stock'])
    game['releaseDate'] = input(f"Release Date (YYYY-MM-DD) [{game['releaseDate']}]: ") or game['releaseDate']

    response = requests.put(f"{BASE_URL}/Games/{game_id}", json=game, verify=False)
    print("Updated! ヅ" if response.status_code == 204 else "Update failed ☹:", response.text)

    input("\nPress Enter to return to the menu...")

#16. update user
def update_user():
    user_id = input("\nEnter User ID to update: ")
    print("Leave fields blank to keep existing values.")

    response = requests.get(f"{BASE_URL}/Users/{user_id}", verify=False)
    if response.status_code != 200:
        print("User not found ☹.")
        return

    user = response.json()
    user['userName'] = input(f"Username [{user['userName']}]: ") or user['userName']
    user['password'] = input(f"Password [{user['password']}]: ") or user['password']
    user['email'] = input(f"Email [{user['email']}]: ") or user['email']
    user['dateOfBirth'] = input(f"DOB [{user['dateOfBirth']}]: ") or user['dateOfBirth']
    user['address'] = input(f"Address [{user['address']}]: ") or user['address']
    user['city'] = input(f"City [{user['city']}]: ") or user['city']
    user['state'] = input(f"State [{user['state']}]: ") or user['state']
    user['zipCode'] = input(f"Zip [{user['zipCode']}]: ") or user['zipCode']
    user['country'] = input(f"Country [{user['country']}]: ") or user['country']

    response = requests.put(f"{BASE_URL}/Users/{user_id}", json=user, verify=False)
    print("User updated! ヅ" if response.status_code == 204 else "Update failed ☹:", response.text)

    input("\nPress Enter to return to the menu...")

#17 update order
def update_order():
    order_id = input("\nEnter Order ID to update: ")
    print("Leave blank to keep current values.")

    response = requests.get(f"{BASE_URL}/Orders/{order_id}", verify=False)
    if response.status_code != 200:
        print("Order not found ☹.")
        return

    order = response.json()
    order['userId'] = int(input(f"User ID [{order['userId']}]: ") or order['userId'])
    order['gameId'] = int(input(f"Game ID [{order['gameId']}]: ") or order['gameId'])
    order['quantity'] = int(input(f"Quantity [{order['quantity']}]: ") or order['quantity'])
    order['orderDate'] = input(f"Order Date [{order['orderDate']}]: ") or order['orderDate']
    order['totalAmount'] = float(input(f"Amount [{order['totalAmount']}]: ") or order['totalAmount'])
    order['status'] = input(f"Status [{order['status']}]: ") or order['status']
    order['paymentMethod'] = input(f"Payment [{order['paymentMethod']}]: ") or order['paymentMethod']
    shipped = input(f"Shipped? (y/n) [{'y' if order['isShipped'] else 'n'}]: ").strip().lower()
    order['isShipped'] = shipped == "y" if shipped else order['isShipped']

    response = requests.put(f"{BASE_URL}/Orders/{order_id}", json=order, verify=False)
    print("Order updated! ヅ" if response.status_code == 204 else "Update failed ☹:", response.text)

    input("\nPress Enter to return to the menu...")

#18 VIEW all rewards
def view_all_rewards():
    url = f"{BASE_URL}/Rewards"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        rewards = response.json()
        print("\nUser Rewards:")
        for r in rewards:
            print(f" - Reward #{r['rewardId']}: User {r['userId']} has {r['points']} points")
    else:
        print("Failed to fetch rewards ☹")

    input("\nPress Enter to return to the menu...")

#19 ADD a new reward
def add_new_reward():
    print("\n🎁 Add New Reward")
    try:
        user_id = int(input("User ID: "))
        points = int(input("Points to assign: "))

        payload = {
            "userId": user_id,
            "points": points
        }

        response = requests.post(f"{BASE_URL}/Rewards", json=payload, verify=False)

        if response.status_code == 201:
            print("Reward added! ヅ")
        else:
            print("Error adding reward ☹:", response.text)

    except ValueError:
        print("Invalid input ☹. Please enter valid numbers.")

    input("\nPress Enter to return to the menu...")

#20
def get_reward_by_id():
    reward_id = input("\nEnter Reward ID to view: ")
    url = f"{BASE_URL}/Rewards/{reward_id}"
    response = requests.get(url, verify=False, auth=AUTH)

    if response.status_code == 200:
        r = response.json()
        print(f"\n🎁 Reward #{r['rewardId']}:")
        print(f" - User ID: {r['userId']}")
        print(f" - Points: {r['points']}")
    else:
        print("Reward not found ☹.")

    input("\nPress Enter to return to the menu...")

#this clears the terminal screen before printing a new menu or info
def clear_console():
    os.system('cls' if os.name == 'nt' else 'clear')

#21 DELETE a reward by ID
def delete_reward():
    reward_id = input("\nEnter Reward ID to delete: ")
    confirm = input(f"Are you sure you want to delete Reward #{reward_id}? (y/n): ").lower()

    if confirm == "y":
        response = requests.delete(f"{BASE_URL}/Rewards/{reward_id}", verify=False)
        if response.status_code == 204:
            print("Reward deleted ヅ.")
        else:
            print("Failed to delete reward ☹.")
    else:
        print("Deletion cancelled.")

    input("\nPress Enter to return to the menu...")

#main loop
def run():
    show_splash()
    input("\nPress Enter to start...")
    while True:
        clear_console()
        print_menu()
        choice = input("Enter your choice: ")

        if choice == "1":
            view_all_games()
        elif choice == "2":
            add_new_game()
        elif choice == "3":
            get_game_by_id()
        elif choice == "4":
            delete_game()
        elif choice == "5":
            search_games_by_name()
        elif choice == "6":
            update_game()

        elif choice == "7":
            view_all_users()
        elif choice == "8":
            add_new_user()
        elif choice == "9":
            get_user_by_id()
        elif choice == "10":
            delete_user()
        elif choice == "11":
            search_users()
        elif choice == "12":
            update_user()

        elif choice == "13":
            view_all_orders()
        elif choice == "14":
            place_order()
        elif choice == "15":
            get_order_by_id()
        elif choice == "16":
            delete_order()
        elif choice == "17":
            update_order()
        elif choice == "18":
            view_all_rewards()
        elif choice == "19":
            add_new_reward()
        elif choice == "20":
            get_reward_by_id()
        elif choice == "21":
            delete_reward()


        elif choice == "0":
            print("👋 Goodbye!")
            break
        else:
            print("Invalid choice ☹. Try again.")

#run the program
if __name__ == "__main__":
    run()
