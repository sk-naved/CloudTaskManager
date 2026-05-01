import { useState, useEffect } from 'react';

function App() {
    const [tasks, setTasks] = useState([]);
    const [title, setTitle] = useState('');
    const [file, setFile] = useState(null);

    // FETCH TASKS
    const fetchTasks = async () => {
        try {
            const response = await fetch('/api/tasks');
            const data = await response.json();
            setTasks(data);
        } catch (err) {
            console.error("Failed to fetch tasks:", err);
        }
    };

    // CREATE TASK
    const handleSubmit = async (e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append('title', title);
        if (file) formData.append('file', file);

        await fetch('/api/tasks', { method: 'POST', body: formData });
        setTitle('');
        setFile(null);
        fetchTasks();
    };

    useEffect(() => { fetchTasks(); }, []);

    return (
        <div style={{ padding: '40px', fontFamily: 'sans-serif' }}>
            <h1>Cloud Task Manager</h1>

            <form onSubmit={handleSubmit} style={{ marginBottom: '20px' }}>
                <input
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    placeholder="Task title"
                    required
                />
                <input type="file" onChange={(e) => setFile(e.target.files[0])} />
                <button type="submit">Add Task</button>
            </form>

            <ul>
                {tasks.map(t => (
                    <li key={t.id}>
                        <strong>{t.title}</strong>
                        {t.fileUrl && <a href={t.fileUrl} target="_blank"> [View File]</a>}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default App;