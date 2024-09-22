import argparse
import json
import os

# ANSI escape codes for colors
GREEN = '\033[92m'  # Green text
RED = '\033[91m'    # Red text
RESET = '\033[0m'   # Reset to default color

def add_entry(file_path, input_value, expected_value):
    # Load existing data from the JSON file
    data = []
    try:
        if os.path.exists(file_path) and os.path.getsize(file_path) > 0:
            with open(file_path, 'r') as file:
                data = json.load(file)
    except json.JSONDecodeError:
        print(RED + 'Failure: JSON file is not valid. Please check the file content.' + RESET)
        return

    # Process input_value into a list of bytes (split logic here)
    input_bytes = []
    
    # Split the input_value string into 3-digit numbers and chars
    for i in range(0, len(input_value), 4):
        amount_str = input_value[i:i+3]  # Extract the 3-digit number (amount)
        char_str = input_value[i+3:i+4]  # Extract the character

        # Convert the amount to a byte and ensure it's between 0 and 255
        amount = int(amount_str)
        if amount < 0 or amount > 255:
            print(RED + f'Failure: Invalid amount {amount}. Must be between 0 and 255.' + RESET)
            return

        # Convert the character to UTF-16 bytes (2 bytes)
        char_bytes = char_str.encode('utf-16le')  # UTF-16 Little Endian

        # Append the amount byte and the character bytes to the output
        input_bytes.append(amount)
        input_bytes.extend(char_bytes)

    # The expected_value is saved as-is, no modification
    new_entry = {
        "Input": input_bytes,                 # The processed input bytes
        "ExpectedOutput": expected_value      # The raw expected output string, saved as-is
    }

    # Append the new entry
    data.append(new_entry)

    # Save back to the JSON file
    try:
        with open(file_path, 'w') as file:
            json.dump(data, file, indent=4)
        print(GREEN + 'Success: Added entry to "{}"'.format(file_path) + RESET)
    except Exception as e:
        print(RED + 'Failure while saving to file: ' + str(e) + RESET)

def main():
    # Set up argument parsing
    parser = argparse.ArgumentParser(description='Add a new entry to a JSON file.')
    parser.add_argument('-f', '--file', required=True, help='The JSON file to update')
    parser.add_argument('-i', '--input', nargs='?', default='', const='', help='The input value (in format: 3-digit amount followed by character, like "003A255B")')
    parser.add_argument('-e', '--expected', nargs='?', default='', const='', help='The expected output value (taken as-is)')

    args = parser.parse_args()

    # Validate input argument length (must be multiples of 4)
    if len(args.input) % 4 != 0:
        print(RED + 'Failure: The input value must be in multiples of 4 characters (3-digit number + 1 char).' + RESET)
        return

    # Call the function to add the entry
    add_entry(args.file, args.input, args.expected)

if __name__ == "__main__":
    main()
