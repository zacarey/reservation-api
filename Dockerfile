FROM postgres:latest

ENV POSTGRES_DB=db
ENV POSTGRES_USER=atreides
ENV POSTGRES_PASSWORD=dune

# Expose the default PostgreSQL port
EXPOSE 5432
