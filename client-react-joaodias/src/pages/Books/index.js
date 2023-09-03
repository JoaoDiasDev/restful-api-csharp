import React, { useState, useMemo, useEffect } from "react";
import "./styles.css";
import logoImage from "../../assets/logo.png";
import { Link, useNavigate } from "react-router-dom";
import { FiPower, FiEdit, FiTrash2 } from "react-icons/fi";
import Api from "../../services/api";

export default function Books() {
  const [books, setBooks] = useState([]);
  const [page, setPage] = useState(1);

  const userName = localStorage.getItem("userName");
  const accessToken = localStorage.getItem("accessToken");

  const authorization = useMemo(
    () => ({
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    }),
    [accessToken]
  );

  const navigate = useNavigate();

  useEffect(() => {
    fetchBooks();
  }, [accessToken]);

  function fetchBooks() {
    Api.get(`api/v1/book/asc/4/${page}`, authorization)
      .then((response) => {
        setBooks((prevBooks) => [...prevBooks, ...response.data.list]);
        setPage((prevPage) => prevPage + 1);
      })
      .catch((error) => {
        console.error("Error fetching books:", error);
      });
  }

  async function deleteBook(id) {
    try {
      await Api.delete(`api/v1/book/${id}`, authorization);

      setBooks(books.filter((book) => book.id !== id));
    } catch (error) {
      alert("Delete failed! Try again!");
    }
  }

  async function editBook(id) {
    try {
      navigate(`/book/new/${id}`);
    } catch (error) {
      alert("Edit failed! Try again!");
    }
  }

  async function logout() {
    try {
      await Api.get(`api/v1/auth/revoke`, authorization);

      localStorage.clear();
      navigate("/");
    } catch (error) {
      alert("Logout failed! Try again!");
    }
  }

  return (
    <div className="book-container">
      <header>
        <img src={logoImage} alt="JoaoDiasDev" />
        <span>
          Welcome,{" "}
          <strong>{userName ? userName.toUpperCase() : "Guest"}</strong>!
        </span>
        <Link className="button" to="/book/new/0">
          Add New Book
        </Link>
        <button onClick={logout} type="button">
          <FiPower size={18} color="#251FC5" />
        </button>
      </header>
      <h1>Registered Books</h1>
      <ul>
        {books.map((book) => (
          <li key={book.id}>
            <strong>Title:</strong>
            <p>{book.title}</p>
            <strong>Author:</strong>
            <p>{book.author}</p>
            <strong>Price:</strong>
            <p>
              {Intl.NumberFormat("pt-BR", {
                style: "currency",
                currency: "BRL",
              }).format(book.price)}
            </p>
            <strong>Release Date:</strong>
            <p>
              {Intl.DateTimeFormat("pt-BR").format(new Date(book.launchDate))}
            </p>

            <button onClick={() => editBook(book.id)} type="button">
              <FiEdit size={20} color="#251FC5" />
            </button>

            <button onClick={() => deleteBook(book.id)} type="button">
              <FiTrash2 size={20} color="#251FC5" />
            </button>
          </li>
        ))}
      </ul>
      <button className="button" onClick={fetchBooks} type="button">
        Load More
      </button>
    </div>
  );
}
