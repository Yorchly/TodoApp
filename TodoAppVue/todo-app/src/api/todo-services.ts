import api from "@/api/api-config";
import { TodoItem } from "@/models/todoModel";

export async function getAllTodos(): Promise<TodoItem[]> {
  const { data } = await api.get("TodoItem");

  return data;
}

export async function createTodo(todoItem: TodoItem): Promise<number> {
  const { data } = await api.post("TodoItem", todoItem);

  return data;
}

export async function updateTodo(todoItem: TodoItem): Promise<boolean> {
  const response = await api.put(`TodoItem/${todoItem.id}`, todoItem);

  return response.status === 204;
}

export async function deleteTodo(todoId: number): Promise<boolean> {
  const response = await api.delete(`TodoItem/${todoId}`);

  return response.status === 200;
}
