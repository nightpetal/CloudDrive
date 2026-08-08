import {
  FaFolder,
  FaFilePdf,
  FaFileWord,
  FaFileImage,
  FaFileAlt,
  FaUpload,
  FaPlus,
  FaSearch,
  FaCloud,
} from "react-icons/fa";
export default function Sidebar() {
  return (
    <div className="col-lg-2 bg-white border-end p-4">
      <button className="btn btn-primary rounded-pill w-100 mb-4">
        <FaPlus className="me-2" />
        Add Folder
      </button>

      <div className="list-group border-0">
        <button className="list-group-item list-group-item-action border-0 rounded active">
          My Drive
        </button>

        <button className="list-group-item list-group-item-action border-0 rounded">
          Shared
        </button>

        <button className="list-group-item list-group-item-action border-0 rounded">
          Recent
        </button>

        <button className="list-group-item list-group-item-action border-0 rounded">
          Trash
        </button>
      </div>
    </div>
  );
}
