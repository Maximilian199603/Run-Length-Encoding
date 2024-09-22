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

    # Split expected value into pairs
    expected_output = split_into_pairs(expected_value)

    # Create a new entry
    new_entry = {
        "Input": input_value,
        "ExpectedOutput": expected_output
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

def split_into_pairs(value):
    # Split the string into a list of two-character long entries
    return [value[i:i + 2] for i in range(0, len(value), 2)]

def main():
    # Set up argument parsing
    parser = argparse.ArgumentParser(description='Add a new entry to a JSON file.')
    parser.add_argument('-f', '--file', required=True, help='The JSON file to update')
    parser.add_argument('-i', '--input', nargs='?', default='', const='', help='The input value')
    parser.add_argument('-e', '--expected', nargs='?', default='', const='', help='The expected output value')

    args = parser.parse_args()

    # Call the function to add the entry
    add_entry(args.file, args.input, args.expected)

if __name__ == "__main__":
    main()
