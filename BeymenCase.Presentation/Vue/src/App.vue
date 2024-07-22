<template>
  <div id="app">
    <h1>App Keys</h1>
    <input type="text" v-model="filter" placeholder="Filter by name" />
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Type</th>
          <th>Value</th>
          <th>Is Active</th>
          <th>Application Name</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in filteredData" :key="item.id">
          <td>{{ item.id }}</td>
          <td>{{ item.name }}</td>
          <td>{{ item.type }}</td>
          <td>{{ item.value }}</td>
          <td>{{ item.isActive }}</td>
          <td>{{ item.applicationName }}</td>
          <td>
            <button @click="editData(item)">Edit</button>
          </td>
        </tr>
      </tbody>
    </table>
    <div>
      <h2>{{ isEditing ? "Edit Data" : "Add Data" }}</h2>
      <form @submit.prevent="isEditing ? updateData() : addData()">
        <input type="text" v-model="formData.name" placeholder="Name" required />
        <input type="text" v-model="formData.type" placeholder="Type" required />
        <input type="text" v-model="formData.value" placeholder="Value" required />
       <span>Is Active <input type="checkbox" v-model="formData.isActive" /> </span> 
        <input type="text" v-model="formData.applicationName" placeholder="Application Name" required />
        <button type="submit">{{ isEditing ? "Update" : "Add" }}</button>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      data: [],
      filter: '',
      isEditing: false,
      formData: {
        id: null,
        name: '',
        type: '',
        value: '',
        isActive: false,
        applicationName: ''
      }
    };
  },
  computed: {
    filteredData() {
      return this.data.filter(item => item.name.includes(this.filter));
    }
  },
  methods: {
    fetchData() {
      this.$axios.get('https://localhost:5001/AppKey/GetAll')
        .then(response => {
          this.data = response.data;
        })
        .catch(error => {
          console.error("There was an error fetching the data!", error);
        });
    },
    addData() {
      console.log(this.formData)
      this.$axios.post('https://localhost:5001/AppKey/Add', this.formData, {headers: {
        'Content-Type': 'application/json'
    }})
        .then(response => {
          this.data.push(response.data);
          this.resetForm();
        })
        .catch(error => {
          console.error("There was an error adding the data!", error);
        });
    },
    editData(item) {
      this.isEditing = true;
      this.formData = { ...item };
    },
    updateData() {
      this.$axios.put(`https://localhost:5001/AppKey/Update`, this.formData, {headers: {
        'Content-Type': 'application/json'
    }})
        .then(response => {
        const index = this.data.findIndex(item => item.id === this.formData.id);
    if (index !== -1) {
      this.data[index] = response.data;
    }
    this.resetForm();
    this.isEditing = false;
        })
        .catch(error => {
          console.error("There was an error updating the data!", error);
        });
    },
    resetForm() {
      this.formData = {
        id: '00000000-0000-0000-0000-000000000000',
        name: '',
        type: '',
        value: '',
        isActive: false,
        applicationName: ''
      };
    }
  },
  mounted() {
    this.fetchData();
  }
};
</script>

<style>
body {
  font-family: Arial, sans-serif;
  background-color: #f9f9f9;
  margin: 0;
  padding: 0;
}

#app {
  max-width: 1200px;
  margin: 20px auto;
  padding: 20px;
  background-color: #ffffff;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

h1 {
  color: #333;
  font-size: 24px;
  margin-bottom: 20px;
}

h2 {
  color: #555;
  font-size: 20px;
  margin-bottom: 20px;
}

form {
  display: grid;
  gap: 12px;
}

.checkbox-container {
  display: flex;
  align-items: center;
  gap: 8px;
}



input[type="text"] {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

input[type="text"]:focus {
  border-color: #007bff;
  outline: none;
}

button {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  background-color: #007bff;
  color: white;
  font-size: 16px;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

th, td {
  padding: 12px;
  text-align: left;
}

th {
  background-color: #007bff;
  color: white;
  font-size: 16px;
}

td {
  background-color: #ffffff;
}

tr:nth-child(even) td {
  background-color: #f2f2f2;
}

button {
  background-color: #28a745;
  border: none;
  padding: 8px 16px;
  color: white;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #218838;
}
@media (max-width: 768px) {
  table {
    font-size: 14px;
  }

  th, td {
    padding: 8px;
  }

  input[type="text"],
  button {
    width: 50%;
    box-sizing: border-box;
    display: inline-block;
  }
}


</style>
