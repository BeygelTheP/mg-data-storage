-- Initialize MG Data Storage database tables
CREATE TABLE IF NOT EXISTS data_storage (
    id TEXT PRIMARY KEY,
    content JSONB NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create index for better query performance
CREATE INDEX IF NOT EXISTS idx_data_storage_created_at ON data_storage(created_at);
CREATE INDEX IF NOT EXISTS idx_data_storage_updated_at ON data_storage(updated_at);

-- Enable row-level security (optional)
-- ALTER TABLE data_storage ENABLE ROW LEVEL SECURITY;