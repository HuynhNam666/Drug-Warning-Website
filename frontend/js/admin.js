let medicines =
  JSON.parse(localStorage.getItem("medicines"));

if (!medicines) {
  medicines = [

    {
      id: 1,
      name: "Paracetamol",
      level: "An toàn"
    },

    {
      id: 2,
      name: "Ibuprofen",
      level: "Thận trọng"
    },

    {
      id: 3,
      name: "Metformin",
      level: "An toàn"
    },

    {
      id: 4,
      name: "Aspirin",
      level: "Nguy cơ cao"
    },

    {
      id: 5,
      name: "Salbutamol",
      level: "An toàn"
    }

  ];

  localStorage.setItem(
    "medicines",
    JSON.stringify(medicines)
  );

}

let diseases =
  JSON.parse(localStorage.getItem("diseases"));

if (!diseases) {
  diseases = [

    "Tiểu đường",
    "Tăng huyết áp",
    "Tim mạch",
    "Suy thận",
    "Hen suyễn"

  ];

  localStorage.setItem(
    "diseases",
    JSON.stringify(diseases)
  );

}

function loadMedicines() {
  const table =
    document.getElementById("medicineTable");

  if (!table) return;

  medicines =
    JSON.parse(
      localStorage.getItem("medicines")
    ) || [];

  let html = "";

  medicines.forEach((m, index) => {

    html += `
    <tr>

        <td>${index + 1}</td>

        <td>${m.name}</td>

        <td>

            <button
            class="btn btn-danger btn-sm"
            onclick="deleteMedicine(${index})">

            Xóa

            </button>

        </td>

    </tr>
    `;

  });

  table.innerHTML = html;

  const count =
    document.getElementById("medicineCount");

  if (count) {

    count.innerText =
      medicines.length;

  }

}

function addMedicine() {
  const input =
    document.getElementById("medicineName");

  const name =
    input.value.trim();

  if (name === "") {

    alert("Nhập tên thuốc");

    return;

  }

  medicines =
    JSON.parse(
      localStorage.getItem("medicines")
    ) || [];

  medicines.push({

    id: medicines.length + 1,
    name: name,
    level: "An toàn"

  });

  localStorage.setItem(
    "medicines",
    JSON.stringify(medicines)
  );

  input.value = "";

  loadMedicines();

}

function deleteMedicine(index) {
  medicines =
    JSON.parse(
      localStorage.getItem("medicines")
    ) || [];

  medicines.splice(index, 1);

  localStorage.setItem(
    "medicines",
    JSON.stringify(medicines)
  );

  loadMedicines();

}

function loadDiseases() {

  const table =
    document.getElementById("diseaseTable");

  if (!table) return;

  diseases =
    JSON.parse(
      localStorage.getItem("diseases")
    ) || [];

  let html = "";

  diseases.forEach((d, index) => {

    html += `
    <tr>

        <td>${index + 1}</td>

        <td>${d}</td>

        <td>

            <button
            class="btn btn-danger btn-sm"
            onclick="deleteDisease(${index})">

            Xóa

            </button>

        </td>

    </tr>
    `;

  });

  table.innerHTML = html;

  const count =
    document.getElementById("diseaseCount");

  if (count) {

    count.innerText =
      diseases.length;

  }

}

function addDisease() {

  const input =
    document.getElementById("diseaseName");

  const name =
    input.value.trim();

  if (name === "") {

    alert("Nhập tên bệnh nền");

    return;

  }

  diseases =
    JSON.parse(
      localStorage.getItem("diseases")
    ) || [];

  diseases.push(name);

  localStorage.setItem(
    "diseases",
    JSON.stringify(diseases)
  );

  input.value = "";

  loadDiseases();

}

function deleteDisease(index) {

  diseases =
    JSON.parse(
      localStorage.getItem("diseases")
    ) || [];

  diseases.splice(index, 1);

  localStorage.setItem(
    "diseases",
    JSON.stringify(diseases)
  );

  loadDiseases();

}

loadMedicines();
loadDiseases();
