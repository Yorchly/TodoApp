<template>
  <div class="mb-4">
    <h1>{{ about.title }}</h1>
    <h2>{{ about.subtitle }} {{ version }}</h2>
  </div>
  <div v-if="isLoading === false">
    <form @submit.prevent="addNewTodo">
      <div class="mb-5">
        <label for="newTodo" class="form-label">New Todo</label>
        <input
          class="form-control"
          id="newTodo"
          placeholder="what's on your mind?"
          v-model="newTodo"
          name="newTodo"
        />
      </div>

      <div class="mb-5 d-flex flex-row justify-content-start">
        <div class="m-2">
          <button type="submit" class="btn btn-primary">Add New Todo</button>
        </div>
        <div class="m-2">
          <button type="button" class="btn btn-danger" @click="removeAllTodos">
            Remove All
          </button>
        </div>
        <div class="m-2">
          <button type="button" class="btn btn-success" @click="markAllDone">
            Mark All Done
          </button>
        </div>
      </div>
    </form>
    <div v-if="todos.length === 0">
      <h3>Empty list 😟</h3>
    </div>
    <div v-else>
      <ul class="list-group">
        <li
          class="list-group-item d-flex flex-row justify-content-between align-items-center"
          v-for="(todo, index) in todos"
          :key="todo.id"
        >
          <h3
            :class="{ mark: todo.done }"
            style="cursor: pointer"
            @click="toggleDone(todo)"
          >
            {{ todo.content }}
          </h3>
          <button
            type="button"
            class="btn btn-warning"
            @click="deleteTodoByIndex(index)"
          >
            ✔️ Done & Remove
          </button>
        </li>
      </ul>
    </div>
  </div>
  <div v-else>
    <div class="d-flex justify-content-center mb-3">
      <h1>Cargando...</h1>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, PropType, onMounted } from "vue";
import { TodoItem } from "@/models/todoModel";
import {
  getAllTodos,
  createTodo as createTodoFromService,
  updateTodo as updateTodoFromService,
  deleteTodo as deleteTodoFromService,
} from "@/api/todo-services";

type Props = {
  title: string;
  subtitle: string;
};

export default defineComponent({
  name: "TodoForm",
  props: {
    about: {
      type: Object as PropType<Props>,
      required: true,
    },
  },
  setup(props) {
    const version = ref("v1");
    const newTodo = ref("");
    const todos = ref<TodoItem[]>([]);
    const isLoading = ref(false);

    async function addNewTodo(): Promise<void> {
      if (!newTodo.value) return;

      await createTodoFromService({
        id: 0,
        content: newTodo.value,
        done: false,
      });

      console.log("new todo: ", newTodo.value);
      newTodo.value = "";

      refreshTodoItems();
    }

    async function refreshTodoItems(): Promise<void> {
      const todoItems = await getAllTodos();
      todos.value = todoItems;
    }

    function toggleDone(todo: TodoItem): void {
      updateTodoDoneState(todo, !todo.done);
    }

    async function updateTodoDoneState(
      todo: TodoItem,
      isDone: boolean
    ): Promise<void> {
      const previousDoneState = todo.done;
      todo.done = isDone;
      const isUpdated = await updateTodoFromService(todo);

      if (!isUpdated) {
        console.log(`TodoItem with id ${todo.id} could not be updated`);
        todo.done = previousDoneState;
      }
    }

    async function deleteTodoByIndex(index: number): Promise<void> {
      const todoItemId = todos.value[index].id;
      const isDeleted = await deleteTodo(todoItemId);

      if (isDeleted) {
        todos.value.splice(index, 1);
      }
    }

    async function deleteTodo(todoItemId: number): Promise<boolean> {
      const isDeleted = await deleteTodoFromService(todoItemId);

      if (!isDeleted) {
        console.log(`TodoItem with id ${todoItemId} could not be deleted`);
      }

      return isDeleted;
    }

    function markAllDone(): void {
      todos.value.forEach((todo: TodoItem) => {
        if (!todo.done) {
          updateTodoDoneState(todo, true);
        }
      });
    }

    function removeAllTodos(): void {
      isLoading.value = true;

      removeTodoItems().then(
        () => {
          refreshTodoItems();
          isLoading.value = false;
        },
        () => (isLoading.value = false)
      );
    }

    async function removeTodoItems(): Promise<void> {
      const promises = todos.value.map(
        async (todo) => await deleteTodo(todo.id)
      );

      await Promise.all(promises);
    }

    onMounted(() => {
      refreshTodoItems();
    });

    return {
      version,
      todos,
      newTodo,
      isLoading,
      addNewTodo,
      toggleDone,
      deleteTodoByIndex,
      markAllDone,
      removeAllTodos,
    };
  },
});
</script>

<style>
.mark {
  text-decoration: line-through;
}

.spinner-style {
  width: 3rem;
  height: 3rem;
}
</style>
