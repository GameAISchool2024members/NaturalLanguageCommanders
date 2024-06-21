import dotenv
import json
import os
import requests

dotenv.load_dotenv()

def step(step_board, moves):
  # Convert the board string into a 2D list for easy manipulation
  board_lines = step_board.strip().split('\n')
  board_grid = [list(line) for line in board_lines]

  # Identify agent positions on the board
  agent_positions = {}
  for row in range(len(board_grid)):
    for col in range(len(board_grid[row])):
      if board_grid[row][col].isdigit():
        agent_positions[int(board_grid[row][col])] = (row, col)

  # Define movement vectors
  move_vectors = {
    'up': (-1, 0),
    'down': (1, 0),
    'left': (0, -1),
    'right': (0, 1)
  }

  # Move each agent according to the specified moves
  for agent, direction in moves.items():
    agent_num = int(agent.split('_')[1])
    if agent_num in agent_positions:
      row, col = agent_positions[agent_num]
      move_vector = move_vectors[direction]
      new_row, new_col = row + move_vector[0], col + move_vector[1]

      # Check if the move is within the bounds and not blocked by a wall
      if (0 <= new_row < len(board_grid) and 0 <= new_col < len(board_grid[0])
          and board_grid[new_row][new_col] == ' '):
        # Move the agent
        board_grid[row][col] = ' '
        board_grid[new_row][new_col] = str(agent_num)
        # Update the agent's position
        agent_positions[agent_num] = (new_row, new_col)

  # Convert the 2D list back to a string representation
  new_board = '\n'.join([''.join(row) for row in board_grid])
  return new_board

def get_command(command_board):
  request_data = {
    "model": "gpt-3.5-turbo",
    "messages": [
      {
        "role": "system", "content": '''
          You are a commander who gives commands in a dramatic,
          game-entertaining way to agents.
          Agents can be used to catch the player on the board.
          You REALLY want to catch the player P
          on the board WITH ALL OF YOUR BLOOD.
          What do you command next?

          When you give your command text, please also ADD at the beginning
          of the output a JSON formatted code that assigns to each agent
          its movement, for example:
          {
            'agent_1': 'down',
            'agent_2': 'up',
            'agent_3': 'left'
          }
        '''
      },
      {"role": "user", "content": command_board},
    ],
    "max_tokens": 150,
    "temperature": 0.7,
  }

  openai_api_key = os.getenv("OPENAI_API_KEY")
  if openai_api_key is None:
    print()
    print("NO OPENAI_API_KEY specified. Please get the key from: https://platform.openai.com/account/api-keys\nThen make an .env file and specify the key like so:\nOPENAI_API_KEY=sk-ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuv")
    print()

  headers = {
    "Authorization": f"Bearer {openai_api_key}",
    "Content-Type": "application/json"
  }

  response = requests.post("https://api.openai.com/v1/chat/completions", headers=headers, json=request_data)
  assert response.status_code

  response_json = response.json()
  full_command = response_json['choices'][0]['message']['content']

  position = full_command.find('}')
  assert position != -1
  moves = full_command[:position + 1].strip()
  text_command = full_command[position + 1:].strip()

  moves = moves.replace('\'', '"')
  print(moves)
  moves = json.loads(moves)
  return moves, text_command



if __name__ == '__main__':

  board = '''
    ---------------------------
    |   1     |  2            |
    |               |         |
    |----------     ----------|
    |                         |
    |---   ------------    ---|
    |                 |       |
    |  P   |          3       |
    |      |                  |
    |      |       |          |
    ---------------------------
  '''
  trace = open('gameplay.txt', 'w')
  print(board, file = trace)

  for _ in range(16):
    moves, text_command = get_command(board)
    board = step(board, moves)
    print(text_command, file = trace)
    print(board, file = trace)
    trace.flush()



