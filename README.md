# Rizal RAG Chat

RAG (retrieval-augmented generation) chat focused on **José Rizal**—ask questions grounded in sources about his life and works.

## Tech stack

- **.NET 8**
- **Microsoft Semantic Kernel and InMemory vector store**
- **Google Gemini** (chat + embeddings)

## Setup

1. Clone this repository.
2. Add a `.env` file in the project root (same folder as the solution or as documented in the app) with:

   | Variable | Description |
   |----------|-------------|
   | `GEMINI_API_KEY` | API key from [Google AI Studio](https://aistudio.google.com/apikey) |
   | `CHAT_MODEL` | Gemini model id for chat (e.g. `gemini-3-flash-preview`) |
   | `EMBEDDING_MODEL` | Gemini model id for embeddings (e.g. `gemini-embedding-2`) |

   Example:

   ```env
   GEMINI_API_KEY=your_key_here
   CHAT_MODEL=gemini-2.0-flash
   EMBEDDING_MODEL=text-embedding-004
   ```

3. Build and run per your solution’s entry project (e.g. `dotnet run`).

> **Security:** Do not commit `.env`. Keep it local and add `.env` to `.gitignore`.
