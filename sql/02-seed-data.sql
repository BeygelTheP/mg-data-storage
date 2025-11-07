-- Sample data for MG Data Storage
INSERT INTO data_storage (id, content) VALUES
('user-001', '{"name": "John Doe", "email": "john@example.com", "role": "admin"}'),
('user-002', '{"name": "Jane Smith", "email": "jane@example.com", "role": "user"}'),
('config-001', '{"theme": "dark", "language": "en", "notifications": true}'),
('product-001', '{"name": "Widget A", "price": 29.99, "category": "tools", "inStock": true}'),
('product-002', '{"name": "Gadget B", "price": 49.99, "category": "electronics", "inStock": false}')
ON CONFLICT (id) DO NOTHING;